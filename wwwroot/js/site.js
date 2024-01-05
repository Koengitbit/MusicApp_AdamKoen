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
function playSong(songId, duration) {
    if (currentTimer) {
        // add song to queue if song alr playing
        songQueue.push({ songId, duration });
    }
    else {
        // no song currently playing, so play song insta
        startPlaying(songId, duration);
    }
}
function startPlaying(songId, duration) {
    var currentTime = 0;
    var formattedDuration = formatDuration(duration);
    var playbackInfo = 'ID: ' + songId + ', Duration: ' + currentTime + '/' + formattedDuration;
    document.getElementById('playingSong').innerText = playbackInfo;
    document.getElementById('playbackBar').style.display = 'block';

    var progressBar = document.getElementById('progressBar');
    progressBar.style.width = '0%';
    // Start timer
    currentTimer = setInterval(function () {
        currentTime++;
        var formattedCurrentTime = formatDuration(currentTime);
        document.getElementById('playingSong').innerText =
            'ID: ' + songId + ', Duration: ' + formattedCurrentTime + '/' + formattedDuration;

        var progressPercentage = (currentTime / duration) * 100;
        progressBar.style.width = progressPercentage + '%';
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
        startPlaying(nextSong.songId, nextSong.duration);
    }
}
function addToQueue(songId, duration) {
    songQueue.push({ songId, duration });
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
        }
    }
}
//Drag and reorder functionality
document.addEventListener('DOMContentLoaded', (event) => {
    const table = document.getElementById('songsTable');

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
        if (targetRow && draggedItem && targetRow.parentNode === draggedItem.parentNode) {
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
        const duration = row.getAttribute('data-duration'); 
        addToQueue(songId, duration);
    });

    // Start playing the first song if no song is currently playing
    if (!currentTimer && songQueue.length > 0) {
        const nextSong = songQueue.shift();
        startPlaying(nextSong.songId, nextSong.duration);
    }
}

function playAllSongsShuffled() {
    let songs = Array.from(document.querySelectorAll('#songsTable .song-row'))
        .map(row => ({
            songId: row.id.split('-')[1],
            duration: row.getAttribute('data-duration')
        }));

    // call shuffle function
    shuffleArray(songs);

    // Add shuffled songs to the queue
    songs.forEach(song => addToQueue(song.songId, song.duration));

    // Start playing the first song if no song is currently playing
    if (!currentTimer && songQueue.length > 0) {
        const nextSong = songQueue.shift();
        startPlaying(nextSong.songId, nextSong.duration);
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
    fetch('/Playlist/Like', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('[name=__RequestVerificationToken]').value
        },
        body: JSON.stringify({ type, itemId })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                console.log(type + ' liked successfully');
                updateLikeButton(type, itemId, data.liked);
            }
            
        })
        .catch(error => console.error('Error:', error));
}

function updateLikeButton(type, itemId, isLiked) {
    const likeButtonSelector = `#${type}-${itemId} .like-button i`;

    const likeButton = document.querySelector(likeButtonSelector);
    if (likeButton) {
        if (isLiked) {
            likeButton.classList.remove('fa-regular');
            likeButton.classList.add('fa-solid');
        } else {
            likeButton.classList.remove('fa-solid');
            likeButton.classList.add('fa-regular');
        }
    }
}


