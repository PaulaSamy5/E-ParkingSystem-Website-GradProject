window.onload = function () {
	console.log(44)
	fetch('/AdminDashboard/GetUsersByGender')
		.then(res => res.json())
		.then(data => {
			const ctx = document.getElementById('genderPieChart').getContext('2d');

			new Chart(ctx, {
				type: 'pie',
				data: {
					labels: ['Male', 'Female'],
					datasets: [{
						label: 'Users by Gender',
						data: [data.male, data.female],
						backgroundColor: ['#36A2EB', '#FF6384'],
						borderColor: ['#ffffff', '#ffffff'],
						borderWidth: 2
					}]
				},
				options: {
					responsive: true,
					plugins: {
						legend: {
							position: 'bottom',
						},
						title: {
							display: true,
							text: 'Users by Gender'
						}
					}
				}
			});
		})
		.catch(err => console.error("Fetch error:", err));
}
async function loadProfitChart(timePeriod = "Day") {
        const response = await fetch(`/AdminDashboard/Profits?timePeriod=${timePeriod}`);
        const data = await response.json();

    const labels = data.map(d => {
        const date = new Date(d.date);
        return `${date.getDate()}/${date.getMonth() + 1}`; // يوم/شهر
    });        const profits = data.map(d => d.profit); // الأرباح

        const ctx = document.getElementById('profitsLineChart').getContext('2d');

        if (window.profitChart) {
            window.profitChart.destroy(); // لو في شارت موجود قبل كده، امسحه
        }

        window.profitChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [{
                    label: '',
                    data: profits,
                    fill: true,
                    borderColor: 'rgba(54, 162, 235, 1)',
                    backgroundColor: 'rgba(54, 162, 235, 0.2)',
                    tension: 0.3,
                    pointRadius: 4,
                    pointHoverRadius: 6
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        display: false
                    },
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                return `Profit: ${context.raw} EGP`;
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'Profits (EGP)',

                        },
                        ticks: {
                            callback: function (value) {
                                if (value >= 1000000) {
                                    return (value / 1000000).toFixed(1) + 'M';
                                } else if (value >= 1000) {
                                    return (value / 1000).toFixed(1) + 'K';
                                }
                                return value;
                            }
                        }
                    },
                    x: {
                        title: {
                            display: true,
                            text: 'Time'
                        }
                    }
                }
            }

        });
    }

    // تحميل الشارت عند فتح الصفحة
    document.addEventListener('DOMContentLoaded', () => {
        loadProfitChart(); // بالافتراضي Day
    });