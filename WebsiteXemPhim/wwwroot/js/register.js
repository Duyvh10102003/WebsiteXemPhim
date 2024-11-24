// Hàm kiểm tra định dạng Email
function isValidEmail(email) {
    var emailRegex = /^[^\s]+@[^\s]+\.[^\s]+$/; // Cập nhật regex để kiểm tra định dạng email chính xác hơn
    return emailRegex.test(email) && email.length <= 30;
}

// Hàm kiểm tra định dạng Password
function isValidPassword(password) {
    var passwordRegex = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[$!%*?&@])[A-Za-z\d$!%*?&@]{8,16}$/;
    return passwordRegex.test(password);
}

// Đăng ký sự kiện khi gửi form
document.getElementById('registerForm').addEventListener('submit', function (event) {
    var email = document.getElementById('email');
    var password = document.getElementById('password');
    var confirmPassword = document.getElementById('confirmPassword');
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

    // Kiểm tra nếu mật khẩu trống
    if (!password.value) {
        password.classList.add('error');
        password.placeholder = 'Vui lòng nhập mật khẩu';
        isError = true;
    } else {
        // Kiểm tra các điều kiện của mật khẩu
        var passwordErrorMessage = '';
        if (password.value.length < 8 || password.value.length > 16) {
            passwordErrorMessage += 'Mật khẩu phải từ 8 đến 16 ký tự. ';
        }
        if (!/[A-Z]/.test(password.value)) {
            passwordErrorMessage += 'Mật khẩu phải có ít nhất 1 chữ hoa. ';
        }
        if (!/[a-z]/.test(password.value)) {
            passwordErrorMessage += 'Mật khẩu phải có ít nhất 1 chữ thường. ';
        }
        if (!/\d/.test(password.value)) {
            passwordErrorMessage += 'Mật khẩu phải có ít nhất 1 số. ';
        }
        if (!/[$!%*?&@]/.test(password.value)) {
            passwordErrorMessage += 'Mật khẩu phải có ít nhất 1 ký tự đặc biệt. ';
        }

        if (passwordErrorMessage) {
            password.classList.add('error');
            password.value = ''; // Xóa mật khẩu nếu không hợp lệ
            password.placeholder = passwordErrorMessage; // Hiển thị thông báo lỗi
            isError = true;
        } else {
            password.classList.remove('error');
        }
    }

    // Kiểm tra nếu xác nhận mật khẩu không khớp
    if (password.value !== confirmPassword.value) {
        confirmPassword.classList.add('error');
        confirmPassword.value = '';
        confirmPassword.placeholder = 'Mật khẩu xác nhận không khớp';
        isError = true;
    } else {
        confirmPassword.classList.remove('error');
    }

    // Ngăn chặn gửi form nếu có lỗi
    if (isError) {
        event.preventDefault();
    }
});
