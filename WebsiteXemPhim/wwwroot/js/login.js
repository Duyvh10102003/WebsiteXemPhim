// Hàm kiểm tra định dạng Email
function isValidEmail(email) {
    var emailRegex = /^[^\s]+[^\s]+\.[^\s]+$/;
    return emailRegex.test(email) && email.length <= 30;
}

// Hàm kiểm tra định dạng Password
function isValidPassword(password) {
    var passwordRegex = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[$!%*?&@])[A-Za-z\d$!%*?&@]{8,16}$/;
    return passwordRegex.test(password);
}

document.getElementById('account').addEventListener('submit', function (event) {
    var email = document.getElementById('email');
    var password = document.getElementById('password');
    var isError = false;

    // Kiểm tra nếu email trống hoặc không đúng định dạng
    if (!email.value) {
        email.classList.add('error');
        email.placeholder = 'Vui lòng nhập tên đăng nhập';
        isError = true;
    } else if (!isValidEmail(email.value)) {
        email.classList.add('error');
        email.value = '';
        email.placeholder = 'Email không hợp lệ';
        isError = true;
    } else {
        email.classList.remove('error');
    }

    // Kiểm tra nếu mật khẩu trống hoặc không đúng định dạng
    if (!password.value) {
        password.classList.add('error');
        password.placeholder = 'Vui lòng nhập mật khẩu';
        isError = true;
    } else if (!isValidPassword(password.value)) {
        password.classList.add('error');
        password.value = '';
        password.placeholder = 'Password không hợp lệ';
        isError = true;
    } else {
        password.classList.remove('error');
    }

    if (isError) {
        event.preventDefault();
    }
});