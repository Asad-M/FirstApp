var dTable;
$(document).ready(function () {
    dTable = $('#myTable').DataTable({
        "ajax": {
            "url": "/Admin/Product/AllProducts"
        },
          columns: [
              
              { "data": 'name' },
              { "data": 'description' },
              { "data": 'price' },
              { "data": "category.name" },
              {
                  "data": "imageURL",
                  "render": function (data) {
                      return `<a href="imageURL">image</a>`
                  }
              },
              {
                  "data": "id",
                  "render": function (data) {
                      return `<a href="/Admin/Product/CreateUpdate?id=${data}"> Edit</a> 
                        <a onClick=RemoveProduct("/Admin/Product/Delete/${data}") href="#"> Delete</a>`
                  }
              }
          ]
    });
});

function RemoveProduct(url) {
    
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'Delete',
                success: function (data) {
                    if (data.success) {
                        dTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message)
                    }
                }
            })
        }
    })
}