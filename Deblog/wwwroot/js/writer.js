var toolbarOptions = [
    ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
    ['blockquote', 'code-block', 'image', 'link'],

    // [{ 'header': 1 }, { 'header': 2 }],               // custom button values
    [{ 'list': 'ordered' }, { 'list': 'bullet' }],
    [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
    [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
    // [{ 'direction': 'rtl' }],                         // text direction

    [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
    [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

    [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
    [{ 'font': [] }],
    [{ 'align': [] }],

    ['clean']                                         // remove formatting button
];

hljs.highlightAll();

var quill = new Quill('#editor', {
    modules: {
        toolbar: toolbarOptions,
        syntax: true,
    },
    theme: editortheme,


    placeholder: 'Compose an your best thoughts...',

    readOnly: readonly,
});

if (!readonly) {
    quill.on('text-change', function (delta, oldDelta, source) {

        document.querySelector(".saveblog").classList.replace("btn-success", "btn-primary");
    });
    quill.focus();
}

