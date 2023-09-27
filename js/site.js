// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
const selectedLabelClass = "sel-label";
// Write your JavaScript code.
const selectedLabel = "sel-label";
function myfunc(id) {
    // get the associated label
    const currentLabel = document.getElementById(id);
    // save the associated label classes before removing
    const currentLabelClass = currentLabel.className;
    const selectedLabels = document.getElementsByClassName(selectedLabel);

    // remove selected class from any selected label
    for (const label of selectedLabels) {
        //console.log(label, label.id, label.className);
        label.classList.remove(selectedLabel);
    }

    // add the selected class if didn't have it
    if (!currentLabelClass.includes(selectedLabel)) {
        currentLabel.className += ` ${selectedLabel}`
    }
}

//multiselectedClass 
const multiselectedClass = "sel-label";
const multiselected = "sel-label";
function multiplefunction(id) {
    const currentLabel = document.getElementById(id);
    const currentLabelClass = currentLabel.className;
    const selectedLabels = document.getElementsByClassName(multiselected);  
    if (!currentLabelClass.includes(multiselected)) {
        currentLabel.className += ` ${multiselected}`
    }
    else {
    currentLabel.classList.remove(multiselected);
    }
}