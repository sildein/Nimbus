/*
 * Events.js
 * This file is a part of Nimbus. Copyright (c) 2017-present Jesse Jones.
 */

// Handle window resize events
window.addEventListener("load", function () {
    var pixels = $(window).height() - 45;
    $("#Explorer").height(pixels);
    $("#InfoPanel").height(pixels);
});
window.addEventListener("resize", function () {
    var pixels = $(window).height() - 45;
    $("#Explorer").height(pixels);
    $("#InfoPanel").height(pixels);
});
