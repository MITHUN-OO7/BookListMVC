var datatable;
$(document).ready(function () {
    loadDataTable();

})
function loadDataTable() {
    dataTable = $('#tblLoad').DataTable({
        "ajax": {
            "url": "/books/getall/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "author", "width": "20%" },
            { "data": "isbn", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/Books/Upsert?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            Edit
                        </a>
                        &nbsp;
                        <a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
                            onclick=Delete('/Books/Delete?id='+${data})>
                            Delete
                        </a>
                        </div>`;
                }, "width": "20%"
            }
        ],
        "language": {
            "emptyTable": "No data found"
        },
        "width": "100%"
    });
}
function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "once deleted, you will not be able to recover",
        icon: "warning",
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.success(data.message);
                    }
                }
            })
        }
    })
}