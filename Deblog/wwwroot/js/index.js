var editortheme = "snow";
var readonly = false;
if (readonly) {
	editortheme = "bubble";
}

document.querySelector(".saveblog").addEventListener("click", () => {
	saveblog();
});

function saveblog() {
	if (readonly) {
		return;
	}
	let content = quill.getContents();
	let blog = JSON.stringify(content);
	//document.querySelector(".saveblog").classList.replace("btn-primary", "btn-success");

	$.post("/Blog/BlogWriter",
		{
			Id: @Model.BlogId,
	BlogStatus: "@Model.BlogStatus",
		BlogAuthor: "@Model.BlogAuthor",
			BlogBody: blogbody
})
				.done(function (data) {
	console.log(data);
	if (data["Status"] == 200) {
		document.querySelector(".saveblog").classList.replace("btn-primary", "btn-success");
	}
	else {
		document.querySelector(".saveblog").classList.replace("btn-primary", "btn-error");
	}
});
		}

loadsaveddata();

function loadsaveddata() {
	let blogbody = @Html.Raw(Json.Serialize(Model.BlogBody));
	let delta = JSON.parse(blogbody);
	quill.setContents(delta);
}