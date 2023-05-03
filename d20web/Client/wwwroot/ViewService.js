ScrollIntoView = (id, behavior, verticalAlignment, horizontalAlignment) => {
    let element = document.getElementById(id);
    if (!!element) { element.scrollIntoView({ behavior: behavior, block: verticalAlignment, inline: horizontalAlignment }); }
}