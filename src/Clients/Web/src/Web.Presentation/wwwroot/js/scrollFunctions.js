const scroll = () => {
    let element = document.getElementById('message-container')

    console.log(element)
    
    element.scrollTop = element.scrollHeight;
}
scroll()