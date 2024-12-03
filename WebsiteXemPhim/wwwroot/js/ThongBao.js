document.addEventListener('DOMContentLoaded', function () {
    const bell = document.getElementById('notificationBell');
    const dropdown = document.getElementById('notificationDropdown');
    const notificationList = document.getElementById('notificationList');
    const deleteAllButton = document.getElementById('deleteAllNotifications');
    const notificationCount = document.getElementById('notificationCount');

    // Hiển thị số lượng thông báo chưa đọc
    fetch('/Notifications/GetUnreadNotificationsCount')
        .then(response => response.json())
        .then(data => {
            if (data.count > 0) {
                notificationCount.innerText = data.count;
                notificationCount.style.display = 'inline-block';
            }
        });

    // Hiển thị dropdown thông báo khi nhấp vào chuông thông báo
    bell.addEventListener('click', function () {
        dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';

        if (dropdown.style.display === 'block') {
            fetch('/Notifications/GetNotifications')
                .then(response => response.json())
                .then(data => {
                    console.log(data); // Kiểm tra dữ liệu nhận được
                    notificationList.innerHTML = ''; // Xóa danh sách thông báo trước đó

                    if (data.length > 0) {
                        data.forEach(notification => {
                            const li = document.createElement('li');
                            li.innerHTML = `
                                <a href="${notification.url}" style="display: flex; align-items: center;">
                                    ${notification.anh ? `<img src="${notification.anh}" alt="Thumbnail">` : ''}
                                    <span>${notification.message}</span>
                                </a>
                                <button onclick="deleteNotification(${notification.id})">&times;</button>
                            `;
                            notificationList.appendChild(li);
                        });

                        // Hiển thị nút "Xóa tất cả" khi có thông báo
                        deleteAllNotifications.style.display = "block";
                    } else {
                        notificationList.innerHTML = '<li>Không có thông báo</li>';
                        // Ẩn nút "Xóa tất cả" nếu không có thông báo
                        deleteAllNotifications.style.display = "none";
                    }
                })
                .catch(error => console.error('Lỗi khi lấy thông báo:', error));
        }
    });

    // Xóa tất cả thông báo khi nhấn vào nút "Xóa tất cả"
    deleteAllButton.addEventListener('click', function () {
        fetch('/Notifications/DeleteAllNotifications', { method: 'POST' })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    notificationList.innerHTML = '<li>Không có thông báo</li>';
                    notificationCount.style.display = 'none';

                    // Ẩn nút "Xóa tất cả" ngay sau khi xóa tất cả
                    deleteAllNotifications.style.display = 'none';
                }
            });
    });
});

// Xóa một thông báo
function deleteNotification(id) {
    fetch(`/Notifications/DeleteNotification?id=${id}`, { method: 'POST' })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                document.querySelector(`button[onclick="deleteNotification(${id})"]`).parentElement.remove();

                // Nếu không còn thông báo
                if (!document.querySelector('#notificationList li')) {
                    document.getElementById('notificationList').innerHTML = '<li>Không có thông báo</li>';
                    document.getElementById('notificationCount').style.display = 'none';

                    // Ẩn nút "Xóa tất cả" khi không còn thông báo
                    deleteAllNotifications.style.display = 'none';
                }
            }
        });
}
