document.addEventListener('DOMContentLoaded', function () {
    var successAlert = document.querySelector('.alert-success');
    if (successAlert) {
        setTimeout(function () {
            successAlert.style.display = 'none';
        }, 5000); // 5000ms = 5 segundos
    }
});

document.addEventListener('DOMContentLoaded', function () {
    var errorAlert = document.querySelector('.alert-danger');
    if (errorAlert) {
        setTimeout(function () {
            errorAlert.style.display = 'none';
        }, 5000); // 5000ms = 5 segundos
    }
});

function togglePasswordVisibility() {
    const passwordInput = document.getElementById('password');
    const eyeIcon = document.querySelector('.eye-icon');
    if (passwordInput.type === 'password') {
        passwordInput.type = 'text';
        eyeIcon.classList.remove('fa-eye');
        eyeIcon.classList.add('fa-eye-slash');
    } else {
        passwordInput.type = 'password';
        eyeIcon.classList.remove('fa-eye-slash');
        eyeIcon.classList.add('fa-eye');
    }
}