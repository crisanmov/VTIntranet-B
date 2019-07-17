function myFunction() {
    document.getElementById("chat-open").style.display = "none";
    document.getElementById("chat-closed").style.display = "flex";
    document.getElementById("chat").style.height = "80vh";
    document.getElementById("chat").style.overflow = "auto";
    document.getElementById("divusers").style.display = "block";
}

function myFunction1() {
    document.getElementById("chat-open").style.display = "flex";
    document.getElementById("chat-closed").style.display = "none";
    document.getElementById("chat").style.height = "40px";
    document.getElementById("chat").style.overflow = "hidden";
    document.getElementById("divusers").style.display = "none";
}