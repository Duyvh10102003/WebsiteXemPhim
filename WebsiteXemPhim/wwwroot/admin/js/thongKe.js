window.onload = function () {
    // Phim Bộ Chart
    var ctxBo = document.getElementById('phimBoChart').getContext('2d');
    var phimBoChart = new Chart(ctxBo, {
        type: 'bar',
        data: {
            labels: phimBoTenDisplay,  // Hiển thị tên trên trục X
            datasets: [{
                label: 'Lượt Xem Phim Bộ',
                data: phimBoLuotXem,
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            },
            tooltips: {
                callbacks: {
                    label: function (tooltipItem) {
                        // Khi di chuột vào cột, hiển thị tên gốc
                        var phimIndex = tooltipItem.index;
                        return phimBoTen[phimIndex] + ': ' + tooltipItem.yLabel;  // Hiển thị tên gốc của phim
                    }
                }
            }
        }
    });

    // Phim Lẻ Chart
    var ctxLe = document.getElementById('phimLeChart').getContext('2d');
    var phimLeChart = new Chart(ctxLe, {
        type: 'bar',
        data: {
            labels: phimLeTenDisplay,  // Hiển thị tên trên trục X
            datasets: [{
                label: 'Lượt Xem Phim Lẻ',
                data: phimLeLuotXem,
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderColor: 'rgba(255, 99, 132, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            },
            tooltips: {
                callbacks: {
                    label: function (tooltipItem) {
                        // Khi di chuột vào cột, hiển thị tên gốc
                        var phimIndex = tooltipItem.index;
                        return phimLeTen[phimIndex] + ': ' + tooltipItem.yLabel;  // Hiển thị tên gốc của phim
                    }
                }
            }
        }
    });

    // Phim Bộ - Most Liked Chart
    var ctxBo2 = document.getElementById('phimBoChart2').getContext('2d');
    var phimBoChart2 = new Chart(ctxBo2, {
        type: 'bar',
        data: {
            labels: phimBoTenDisplay,  // Hiển thị tên trên trục X
            datasets: [{
                label: 'Phim Bộ được yêu thích',
                data: phimBoLike,
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            },
            tooltips: {
                callbacks: {
                    label: function (tooltipItem) {
                        // Khi di chuột vào cột, hiển thị tên gốc
                        var phimIndex = tooltipItem.index;
                        return phimBoTen[phimIndex] + ': ' + tooltipItem.yLabel;  // Hiển thị tên gốc của phim
                    }
                }
            }
        }
    });

    // Phim Lẻ - Most Liked Chart
    var ctxLe2 = document.getElementById('phimLeChart2').getContext('2d');
    var phimLeChart2 = new Chart(ctxLe2, {
        type: 'bar',
        data: {
            labels: phimLeTenDisplay,  // Hiển thị tên trên trục X
            datasets: [{
                label: 'Phim lẻ được yêu thích nhất',
                data: phimLeLike,
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderColor: 'rgba(255, 99, 132, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            },
            tooltips: {
                callbacks: {
                    label: function (tooltipItem) {
                        // Khi di chuột vào cột, hiển thị tên gốc
                        var phimIndex = tooltipItem.index;
                        return phimLeTen[phimIndex] + ': ' + tooltipItem.yLabel;  // Hiển thị tên gốc của phim
                    }
                }
            }
        }
    });
};
