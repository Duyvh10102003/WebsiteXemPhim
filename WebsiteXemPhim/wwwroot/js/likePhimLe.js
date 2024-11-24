document.getElementById('favorite-link').addEventListener('click', function () {
    var phimLeId = this.getAttribute('data-phimle-id');
    var isFavorite = this.getAttribute('data-is-favorite') === 'true';

    if (!isFavorite) {
        addToFavorites(phimLeId, this);
    } else {
        removeFromFavorites(phimLeId, this);
    }
});

async function addToFavorites(phimLeId, link) {
    try {
        let response = await fetch('/HopPhim/AddPhimLe?phimleid=' + phimLeId, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ phimLeId: phimLeId })
        });

        if (response.ok) {
            console.log('Phim đã được thêm vào danh sách yêu thích.');
            link.setAttribute('data-is-favorite', 'true');
            link.innerHTML = '<i class="fa fa-heart"></i> Đã thích';
            toastr.success('Phim đã được thêm vào danh sách yêu thích.');
        } else {
            const errorText = await response.text();
            console.error('Lỗi khi thêm phim vào danh sách yêu thích:', errorText);
            toastr.error('Đã xảy ra lỗi khi thêm phim vào danh sách yêu thích. Vui lòng thử lại sau.');
        }
    } catch (error) {
        console.error('Lỗi kết nối:', error);
        toastr.error('Đã xảy ra lỗi khi thêm phim vào danh sách yêu thích. Vui lòng thử lại sau.');
    }
}

function removeFromFavorites(phimLeId, link) {
    fetch('/HopPhim/RemovePhimLe?phimleid=' + phimLeId, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ phimLeId: phimLeId })
    })
        .then(response => {
            return response.text().then(text => {
                console.log('Trạng thái phản hồi:', response.status);
                console.log('Nội dung phản hồi:', text);

                if (response.ok) {
                    console.log('Phim đã được loại khỏi danh sách yêu thích.');
                    link.setAttribute('data-is-favorite', 'false');
                    link.innerHTML = '<i class="fa fa-heart-o"></i> Yêu thích';

                    toastr.options = {
                        "closeButton": true,
                        "debug": false,
                        "newestOnTop": true,
                        "progressBar": true,
                        "positionClass": "toast-top-right",
                        "preventDuplicates": false,
                        "onclick": null,
                        "showDuration": "300",
                        "hideDuration": "1000",
                        "timeOut": "5000",
                        "extendedTimeOut": "1000",
                        "showEasing": "swing",
                        "hideEasing": "linear",
                        "showMethod": "fadeIn",
                        "hideMethod": "fadeOut"
                    };
                    toastr.error('Phim đã được loại khỏi danh sách yêu thích.', 'Thông báo', {
                        iconClass: 'toast-icon toast-error'
                    });
                } else {
                    console.error('Lỗi khi loại phim khỏi danh sách yêu thích:', text);
                    toastr.error('Đã xảy ra lỗi khi loại phim khỏi danh sách yêu thích. Vui lòng thử lại sau.', 'Lỗi');
                }
            });
        })
        .catch(error => {
            console.error('Lỗi kết nối:', error);
            toastr.error('Đã xảy ra lỗi khi loại phim khỏi danh sách yêu thích. Vui lòng thử lại sau.', 'Lỗi');
        });
}
