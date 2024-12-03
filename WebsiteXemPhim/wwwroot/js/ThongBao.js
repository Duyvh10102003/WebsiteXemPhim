document.addEventListener('DOMContentLoaded', function () {
    const bell = document.getElementById('notificationBell');
    const dropdown = document.getElementById('notificationDropdown');
    const notificationList = document.getElementById('notificationList');
    const deleteAllButton = document.getElementById('deleteAllNotifications');

    // Hiển thị số lượng thông báo
    fetch('/Notifications/GetUnreadNotificationsCount')
        .then(response => response.json())
        .then(data => {
            if (data.count > 0) {
                const badge = document.getElementById('notificationCount');
                badge.innerText = data.count;
                badge.style.display = 'inline-block';
            }
        });

    // Hiển thị dropdown thông báo
    bell.addEventListener('click', function () {
        dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';

        if (dropdown.style.display === 'block') {
            fetch('/Notifications/GetNotifications')
                .then(response => response.json())
                .then(data => {
                    console.log(data); // Kiểm tra dữ liệu nhận được
                    notificationList.innerHTML = '';

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
                    } else {
                        notificationList.innerHTML = '<li>Không có thông báo</li>';
                    }
                })
                .catch(error => console.error('Error fetching notifications:', error));
        }
    });



    // Xóa tất cả thông báo
    deleteAllButton.addEventListener('click', function () {
        fetch('/Notifications/DeleteAllNotifications', { method: 'POST' })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    notificationList.innerHTML = '<li>Không có thông báo</li>';
                    document.getElementById('notificationCount').style.display = 'none';
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
                }
            }
        });
}
