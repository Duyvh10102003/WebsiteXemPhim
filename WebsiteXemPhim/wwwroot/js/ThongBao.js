document.addEventListener('DOMContentLoaded', function () {
    fetch('/Notifications/GetUnreadNotificationsCount')
        .then(response => response.json())
        .then(data => {
            if (data.count > 0) {
                const badge = document.getElementById('notificationCount');
                badge.innerText = data.count;
                badge.style.display = 'inline-block';
            }
        });
});
document.addEventListener('DOMContentLoaded', function () {
    const bell = document.getElementById('notificationBell');
    const dropdown = document.getElementById('notificationDropdown');
    const notificationList = document.getElementById('notificationList');

    // Tải số lượng thông báo chưa đọc
    fetch('/Notifications/GetUnreadNotificationsCount')
        .then(response => response.json())
        .then(data => {
            if (data.count > 0) {
                const badge = document.getElementById('notificationCount');
                badge.innerText = data.count;
                badge.style.display = 'inline-block';
            }
        });

    // Hiển thị dropdown khi bấm vào chuông
    bell.addEventListener('click', function () {
        if (dropdown.style.display === 'block') {
            dropdown.style.display = 'none';
        } else {
            dropdown.style.display = 'block';

            // Tải danh sách thông báo
            fetch('/Notifications/GetNotifications')
                .then(response => response.json())
                .then(data => {
                    notificationList.innerHTML = ''; // Xóa nội dung cũ

                    if (data.length > 0) {
                        data.forEach(notification => {
                            const li = document.createElement('li');
                            li.innerHTML = `
                                <a href="${notification.url}">
                                    ${notification.message}
                                </a>
                            `;
                            notificationList.appendChild(li);
                        });
                    } else {
                        notificationList.innerHTML = '<li>Không có thông báo</li>';
                    }
                });
        }
    });
});

