// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
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
        playNextSong();
    }
}