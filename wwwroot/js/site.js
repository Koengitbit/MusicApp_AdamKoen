//Code to format duration to mm:ss
function formatDuration(seconds) {
    var minutes = Math.floor(seconds / 60);
    var remainingSeconds = seconds % 60

    if (remainingSeconds < 10) {
        remainingSeconds = '0' + remainingSeconds;
    }

    return minutes + ':' + remainingSeconds;
}
var songQueue = [];
var currentTimer = null;
//Code to play song in overview currently
function playSong(songId, songTitle, duration) {
    if (currentTimer) {
        // add song to queue if song already playing
        songQueue.push({ songId, songTitle, duration });
    }
    else {
        // no song currently playing, so play song instantly
        startPlaying(songId, songTitle, duration);
    }
}
function startPlaying(songId,songTitle, duration) {
    var currentTime = 0;
    var formattedDuration = formatDuration(duration);
    var playbackInfo = songTitle + ', Duration: ' + currentTime + '/' + formattedDuration;
    document.getElementById('playingSong').innerText = playbackInfo;
    document.getElementById('playbackBar').style.display = 'block';

    var progressBar = document.getElementById('progressBar');
    progressBar.style.width = '0%';
    // Start timer
    currentTimer = setInterval(function () {
        currentTime++;
        var formattedCurrentTime = formatDuration(currentTime);
        document.getElementById('playingSong').innerText =
         songTitle  + ', Duration: ' + formattedCurrentTime + '/' + formattedDuration;

        var progressPercentage = (currentTime / duration) * 100;
        progressBar.style.width = progressPercentage + '%';
        //update playback state
        savePlaybackState(songId, songTitle, currentTime, duration);
        
        // Stop the timer
        if (currentTime >= duration) {
            clearInterval(currentTimer);
            progressBar.style.width = '100%';
            playNextSong();
        }
    }, 1000); // Update each sec
}


function playNextSong() {
    if (songQueue.length > 0) {
        var nextSong = songQueue.shift(); // Remove the first song from the queue
        startPlaying(nextSong.songId, nextSong.songTitle, nextSong.duration);
    }
    else {
        // Queue is empty, clear the playback state
        localStorage.removeItem('playbackState');
    }
}

function addToQueue(songId, songTitle, duration) {
    songQueue.push({ songId, songTitle, duration });
}

function skipSong() {
    if (currentTimer) {
        clearInterval(currentTimer);
        if (songQueue.length > 0) {
            playNextSong();
        } else {
            // Hide the playback bar and reset the current timer
            document.getElementById('playbackBar').style.display = 'none';
            currentTimer = null;
            document.getElementById('playingSong').innerText = '';
            localStorage.removeItem('playbackState');
        }
    }
}
//Drag and reorder functionality
document.addEventListener('DOMContentLoaded', (event) => {
    const table = document.getElementById('songsTable');
    //If on a page where there's no songTable it would give an error this makes it return nothing so there's no error.
    if (!table) {
        return;
    }
    let draggedItem = null;

    table.addEventListener('dragstart', (e) => {
        console.log("Dragstart");
        draggedItem = e.target;
    });

    table.addEventListener('dragover', (e) => {
        console.log("Dragover");
        e.preventDefault();
    });

    table.addEventListener('drop', (e) => {
        e.preventDefault();
        
        let targetRow = e.target.closest('tr');
        //make sure the dragged item and the target are both song rows
        if (targetRow && draggedItem && targetRow.matches('.song-row') && draggedItem.matches('.song-row')) {
            swapElements(draggedItem, targetRow);
            updateSongOrder();
        }
    });
});

function updateSongOrder() {
    const table = document.getElementById('songsTable');
    const playlistId = table.getAttribute('data-playlist-id');
    const songIds = Array.from(document.querySelectorAll('#songsTable .song-row'))
        .map(row => row.id.split('-')[1]); // Extracting song IDs

    console.log(songIds);

    fetch(`/Playlist/UpdateSongOrder?playlistId=${playlistId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(songIds),
    })
        .then(response => response.json())
        .then(data => {
            console.log('Order updated:', data);
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}
//Helper function for drag and reorder function
function swapElements(elem1, elem2) {
    const parent1 = elem1.parentNode;
    const next1 = elem1.nextSibling === elem2 ? elem1 : elem1.nextSibling;
    const parent2 = elem2.parentNode;
    const next2 = elem2.nextSibling === elem1 ? elem2 : elem2.nextSibling;

    parent1.insertBefore(elem2, next1);
    parent2.insertBefore(elem1, next2);
}

//Playlist function
function playAllSongs() {
    document.querySelectorAll('#songsTable .song-row').forEach(row => {
        const songId = row.id.split('-')[1];
        const songTitle = row.getAttribute('title');
        const duration = row.getAttribute('data-duration'); 
        addToQueue(songId, songTitle ,duration);
    });

    // Start playing the first song if no song is currently playing
    if (!currentTimer && songQueue.length > 0) {
        const nextSong = songQueue.shift();
        startPlaying(nextSong.songId, nextSong.songTitle, nextSong.duration);
    }
}

function playAllSongsShuffled() {
    let songs = Array.from(document.querySelectorAll('#songsTable .song-row'))
        .map(row => ({
            songId: row.id.split('-')[1],
            songTitle: row.getAttribute('title'),
            duration: row.getAttribute('data-duration')
        }));

    // call shuffle function
    shuffleArray(songs);

    // Add shuffled songs to the queue
    songs.forEach(song => addToQueue(song.songId, song.songTitle , song.duration));

    // Start playing the first song if no song is currently playing
    if (!currentTimer && songQueue.length > 0) {
        const nextSong = songQueue.shift();
        startPlaying(nextSong.songId, nextSong.songTitle, nextSong.duration);
    }
}

// Utility function to shuffle an array
function shuffleArray(array) {
    for (let i = array.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [array[i], array[j]] = [array[j], array[i]];
    }
}
// Like
function likeItem(type, itemId) {
    let headers = {
        'Content-Type': 'application/json'
    };
    const tokenElement = document.querySelector('[name=__RequestVerificationToken]');
    if (tokenElement) {
        headers['RequestVerificationToken'] = tokenElement.value;
    }

    fetch('/Playlist/Like', {
        method: 'POST',
        headers: headers,
        body: JSON.stringify({ type, itemId })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                console.log(type + ' liked successfully');
                console.log("Type:", type, "Item ID:", itemId, "data liked:", data.liked);

                updateLikeButton(type, itemId, data.liked);
            }
            
        })
        .catch(error => console.error('Error:', error));
}

function updateLikeButton(type, itemId, isLiked) {
        const likeButtonSelector = `#${type}-${itemId} .like-button i`;
        console.log(likeButtonSelector);
        const likeButton = document.querySelector(likeButtonSelector);
        console.log(likeButton);
        if (likeButton) {
            console.log("I got here");
            if (isLiked) {
                likeButton.classList.remove('fa-regular');
                likeButton.classList.add('fa-solid');
            } else {
                likeButton.classList.remove('fa-solid');
                likeButton.classList.add('fa-regular');
            }
        }
}

function playAlbum(startingSongId, startingSongTitle, duration) {
    // Clear the current queue and timer
    songQueue = [];
    if (currentTimer) {
        clearInterval(currentTimer);
        currentTimer = null;
    }

    // Start playing the selected song
    startPlaying(startingSongId, startingSongTitle, duration);

    // Queue the rest of the album starting from the selected song
    let startQueueing = false;
    document.querySelectorAll('#songsTable .song-row').forEach(row => {
        const songId = row.id.split('-')[1];
        const songTitle = row.getAttribute('title');
        const songDuration = row.getAttribute('data-duration');

        if (songId === startingSongId) {
            startQueueing = true;
        } else if (startQueueing) {
            songQueue.push({ songId, songTitle, duration: songDuration });
        }
    });
}

function savePlaybackState(songId, songTitle,currentTime, duration) {
    const playbackState = {
        songId: songId,
        songTitle, songTitle,
        currentTime: currentTime,
        duration: duration,
        songQueue: songQueue // Save the entire song queue
    };
    localStorage.setItem('playbackState', JSON.stringify(playbackState));
}


document.addEventListener('DOMContentLoaded', function () {
    const savedState = localStorage.getItem('playbackState');
    if (savedState) {
        const playbackState = JSON.parse(savedState);
        songQueue = playbackState.songQueue || []; // Restore the song queue
        if (playbackState.songId) {
            startPlayingFromSpecificTime(playbackState.songId, playbackState.songTitle, playbackState.duration, playbackState.currentTime);
        }
    }
});



function playSongFromSpecificTime(songId, songTitle,duration, startTime) {
    if (currentTimer) {
        // If a song is already playing, add this song to the queue with the specific start time
        songQueue.push({ songId, songTitle,duration, startTime });
    } else {
        // No song is currently playing, so start this song from the specific start time
        startPlayingFromSpecificTime(songId, songTitle, duration, startTime);
    }
}

function startPlayingFromSpecificTime(songId, songTitle, duration, startTime) {
    var currentTime = startTime || 0; // If startTime is undefined, default to 0
    var formattedDuration = formatDuration(duration);
    var formattedCurrentTime = formatDuration(currentTime);

    var playbackInfo = songTitle + ', Duration: ' + formattedCurrentTime + '/' + formattedDuration;
    document.getElementById('playingSong').innerText = playbackInfo;
    document.getElementById('playbackBar').style.display = 'block';

    var progressBar = document.getElementById('progressBar');
    progressBar.style.width = (currentTime / duration) * 100 + '%';

    // Start or restart the timer
    currentTimer = setInterval(function () {
        currentTime++;
        var formattedCurrentTime = formatDuration(currentTime);
        document.getElementById('playingSong').innerText =
            songTitle + ', Duration: ' + formattedCurrentTime + '/' + formattedDuration;

        var progressPercentage = (currentTime / duration) * 100;
        progressBar.style.width = progressPercentage + '%';

        // Stop the timer if the song has finished
        if (currentTime >= duration) {
            clearInterval(currentTimer);
            progressBar.style.width = '100%';
            playNextSong();
        }

        savePlaybackState(songId, songTitle,currentTime, duration);
    }, 1000); // Update every second
}
function saveUserPreference(shouldNotMaintainQueue) {
    localStorage.setItem('shouldNotMaintainQueue', shouldNotMaintainQueue);
}
function handleQueuePreferenceChange(isChecked) {
    saveUserPreference(isChecked);
}

window.addEventListener('beforeunload', function () {
    localStorage.setItem('appClosing', 'true');
});

document.addEventListener('DOMContentLoaded', function () {
    // Set the 'appOpened' flag in session storage
    sessionStorage.setItem('appOpened', 'true');

    // Check if the app was previously in a closing state
    const appClosing = localStorage.getItem('appClosing');
    const shouldNotMaintainQueue = localStorage.getItem('shouldNotMaintainQueue') === 'true';

    if (appClosing && shouldNotMaintainQueue) {
        // App was closing and user chose not to maintain the queue
        localStorage.removeItem('playbackState');
    }

    // Clear the 'appClosing' flag as we are now opening/reloading the app
    localStorage.removeItem('appClosing');

    // Restore the state of the checkbox based on user preference
    var shouldNotMaintainQueueCheckbox = document.getElementById('shouldNotMaintainQueue');
    if (shouldNotMaintainQueueCheckbox) {
        shouldNotMaintainQueueCheckbox.checked = shouldNotMaintainQueue;
    }
});
