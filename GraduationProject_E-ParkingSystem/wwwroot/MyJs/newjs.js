// Function to show a specific table and add 'active' to the corresponding button
function showTable(tableId, button) {
    // Hide both tables first
    document.getElementById("Car-table").style.display = "none";
    document.getElementById("Bus-table").style.display = "none";

    // Show selected table
    document.getElementById(tableId).style.display = "block";

    // Remove active class from all buttons
    document.querySelectorAll(".changableButtons .btn").forEach(btn => btn.classList.remove("active"));

    // Add active class to the clicked button
    button.classList.add("active");
}

// Function to store the active button state when modal is closed
function setActiveButtonOnClose() {
    let activeButton = document.querySelector('.changableButtons .btn.active');
    if (activeButton && activeButton.textContent.includes('Show Buses Slots')) {
        localStorage.setItem('activeButton', 'Bus');
    } else {
        localStorage.setItem('activeButton', 'Car');
    }
}

// Function to restore the active button state when page is loaded or refreshed
function restoreActiveButtonOnLoad() {
    // Hide both initially to prevent flicker
    document.getElementById("Car-table").style.display = "none";
    document.getElementById("Bus-table").style.display = "none";

    let activeState = localStorage.getItem('activeButton');

    if (activeState === 'Bus') {
        showTable('Bus-table', document.getElementById('btnShowBuses'));
    } else {
        showTable('Car-table', document.getElementById('btnShowCars'));
    }

    // Optional: remove the key after use
    localStorage.removeItem('activeButton');
}

// Call this when the DOM is fully loaded
document.addEventListener("DOMContentLoaded", function () {
    restoreActiveButtonOnLoad();
});

// Handle close & refresh button in modal
document.getElementById('closeFooterBtn').addEventListener("click", function () {
    setActiveButtonOnClose();
    clearInput();
    setTimeout(() => location.reload(), 0); // Faster reload
});

//function validateInput() {
//    const inputField = document.getElementById("slotNameInput");
//    const errorMessage = document.getElementById("nameValidationMessageu");
//    const btn = document.getElementById("mybt");

//    const slotName = inputField.value.trim();
//    const validFormat = /^[A-Z]\d+$/;
//    const firstLetter = slotName.charAt(0).toUpperCase();

//    // Reset error message before validation
//    errorMessage.textContent = "";

//    // Check if slot name is empty
//    if (slotName === "") {
//        errorMessage.textContent = "Slot name cannot be empty.";
//        btn.disabled = true;
//        return;
//    }

//    // Check format: Letter followed by numbers
//    if (!validFormat.test(slotName)) {
//        errorMessage.textContent = "Slot name must start with a letter followed by a number (e.g., A1, B23).";
//        btn.disabled = true;
//        return;
//    }

//    // Check the allowed letters based on type (car/bus)
//    if (currentType === "car" && !/^[A-C]$/.test(firstLetter)) {
//        errorMessage.textContent = "Allowed letters for car: A, B, C";
//        btn.disabled = true;
//        return;
//    }

//    if (currentType === "bus" && !/^[X-Z]$/.test(firstLetter)) {
//        errorMessage.textContent = "Allowed letters for bus: X, Y, Z";
//        btn.disabled = true;
//        return;
//    }

//    // If all validations pass
//    btn.disabled = false;
//    errorMessage.textContent = "";
//}

//let currentType = null;       // car or bus
//let isUpdateMode = false;     // false = add, true = update
function openUpdateWindow(slotId, type) {
    console.log("openUpdateWindow called with slotId:", slotId); // This will show if the function is being called
    //let currentType = '';
    let currentType = type;

    // Update the "testtype" paragraph with the type
    document.getElementById("testtype").textContent = `${currentType} is the type`;

    let nameCell = document.getElementById(`slot-name-${slotId}`);
    let slotName = nameCell ? nameCell.textContent.trim() : '';

    if (!nameCell) {
        console.error(`❌ Slot name cell not found for ID: ${slotId}`);
        return;
    }

    console.log("currentType before check:", currentType);
    if (currentType == null) {
        console.log("not found");
    } else {
        console.log("founded");
    }

    console.log("🚀 sssssOpening Modal with ID:", slotId, "Name:", slotName, "type:", currentType);

    document.getElementById("slotIdInput").value = slotId;
    document.getElementById("slotNameInput").value = slotName;
    document.getElementById("nnn").textContent = slotName || "Unknown Slot";

    // Show the modal
    $("#updateSlotModal").modal("show");
}

let selectedSlotId = null;
let selectedButton = null;

function showDeleteModal(slotId, btn) {
    selectedSlotId = slotId;  // Store the slot ID
    selectedButton = btn;      // Store the button reference
    $('#deleteConfirmModal').modal('show'); // Show the modal
}

document.getElementById("confirmDeleteBtn").addEventListener("click", function () {
    if (selectedSlotId !== null && selectedButton !== null) {
        deleteSlot(selectedSlotId, selectedButton);
        $('#deleteConfirmModal').modal('hide'); // Hide the modal after confirming
    }
});

function deleteSlot(slotId, btn) {
    let row = btn.closest("tr"); // Find the row containing the button
    let table = row.closest("table"); // Find the table containing the row

    fetch(`/AdminDashboard/DeleteSlot/${slotId}`, {
        method: 'DELETE'
    })
        .then(response => {
            if (response.ok) {
                row.remove(); // Remove the row from the table
                updateCount(table); // Update the count after deletion
            } else {
                alert("Failed to delete the slot.");
            }
        })
        .catch(error => console.error('Error:', error));
}
function updateCount(table) {
    let rows = table.querySelectorAll("tbody tr"); // Get all rows in tbody
    rows.forEach((row, index) => {
        let countCell = row.querySelector("th[scope='row']"); // Find the count cell
        if (countCell) {
            countCell.textContent = index + 1; // Update count (assuming it starts from 1)
        }
    });
}

let abortController; // متغير عام لحفظ آخر طلب

//document.getElementById("slotNameInput").addEventListener("input", function () {
//    let slotName = this.value.trim();
//    let slotId = document.getElementById("slotIdInput").value;
//    let errorMessage = document.getElementById("nameValidationMessageu");
//    let btn = document.getElementById("mybt");

//    // Reset error message before validation
//    errorMessage.textContent = "";

//    // Regular expression: Starts with a letter, followed by one or more digits
//    let validFormat = /^[A-Z]\d+$/;

//    if (slotName === "") {
//        errorMessage.textContent = "Slot name cannot be empty.";
//        btn.disabled = true;
//        return;
//    }

//    if (!validFormat.test(slotName)) {
//        errorMessage.textContent = "Slot name must start with a letter followed by a number (e.g., A1, B23).";
//        btn.disabled = true;
//        return;
//    }
//    slotName.validateInput();
//    // ✅ إلغاء أي طلب سابق قبل إرسال طلب جديد
//    if (abortController) {
//        abortController.abort(); // إلغاء الطلب القديم
//    }
//    abortController = new AbortController(); // إنشاء كائن جديد

//    // ✅ إرسال الطلب الجديد
//    fetch(`/AdminDashboard/CheckSlotName?name=${encodeURIComponent(slotName)}&id=${slotId}`, { signal: abortController.signal })
//        .then(response => response.json())
//        .then(data => {
//            if (data.exists) {
//                errorMessage.textContent = "This slot name is already taken.";
//                btn.disabled = true;
//            } else {
//                errorMessage.textContent = "";
//                btn.disabled = false;
//            }
//        })
//        .catch(error => {
//            if (error.name !== "AbortError") { // تجاهل الأخطاء الناتجة عن الإلغاء
//                console.error("Error checking slot name:", error);
//                errorMessage.textContent = "Error checking name. Try again.";
//                btn.disabled = true;
//            }
//        });
//});

//document.getElementById("slotNameInput").addEventListener("input", function () {
//    let slotName = this.value.trim();
//    let slotId = document.getElementById("slotIdInput").value;
//    let errorMessage = document.getElementById("nameValidationMessageu");
//    let btn = document.getElementById("mybt");

//    // Reset error message before validation
//    errorMessage.textContent = "";

//    // Regular expression: Starts with a letter, followed by one or more digits
//    let validFormat = /^[A-Z]\d+$/;
    
//    // Get the current type (car or bus)
//    let currentType = document.getElementById("testtype") ? document.getElementById("testtype").textContent.toLowerCase() : "";
//    console.log("Current type:", currentType);  // Added for debugging

//    let firstLetter = slotName.charAt(0).toUpperCase();

//    // Check if slot name is empty
//    if (slotName === "") {
//        errorMessage.textContent = "Slot name cannot be empty.";
//        btn.disabled = true;
//        return;
//    }

//    // Check format: Letter followed by numbers
//    if (!validFormat.test(slotName)) {
//        errorMessage.textContent = "Slot name must start with a letter followed by a number (e.g., A1, B23).";
//        btn.disabled = true;
//        return;
//    }

//    // Check allowed letters based on type (car/bus)
//    if (currentType === "car" && !/^[A-C]$/.test(firstLetter)) {
//        errorMessage.textContent = "Allowed letters for car: A, B, C";
//        btn.disabled = true;
//        return;
//    }

//    if (currentType === "bus" && !/^[X-Z]$/.test(firstLetter)) {
//        errorMessage.textContent = "Allowed letters for bus: X, Y, Z";
//        btn.disabled = true;
//        return;
//    }

//    // ✅ Cancel any previous request before making a new one
//    if (abortController) {
//        abortController.abort(); // Cancel the previous request
//    }
//    abortController = new AbortController(); // Create a new abort controller

//    // ✅ Send a new request
//    fetch(`/AdminDashboard/CheckSlotName?name=${encodeURIComponent(slotName)}&id=${slotId}`, { signal: abortController.signal })
//        .then(response => response.json())
//        .then(data => {
//            if (data.exists) {
//                errorMessage.textContent = "This slot name is already taken.";
//                btn.disabled = true;
//            } else {
//                errorMessage.textContent = "";
//                btn.disabled = false;
//            }
//        })
//        .catch(error => {
//            if (error.name !== "AbortError") { // Ignore errors from abort
//                console.error("Error checking slot name:", error);
//                errorMessage.textContent = "Error checking name. Try again.";
//                btn.disabled = true;
//            }
//        });
//});

document.getElementById("updateForm").addEventListener("submit", function (event) {
    event.preventDefault();

    let slotId = document.getElementById("slotIdInput").value;
    let slotName = document.getElementById("slotNameInput").value;
    document.getElementById("nnn").value;

    fetch(`/AdminDashboard/WindowForUpdateTheSlot`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ id: slotId, slotName: slotName })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // ✅ تحديث القيمة في الجدول
                let nameCell = document.getElementById(`slot-name-${slotId}`);
                if (nameCell) {
                    nameCell.textContent = slotName;
                }

                // ✅ قفل المودال بعد التحديث
                $("#updateSlotModal").modal("hide");
            } else {
                console.error("❌ Error: ", data.message);
            }
        })
        .catch(error => {
            console.error("❌ Error updating slot:", error);
        });
});

let scrolll = document.getElementById('scroll')
window.onscroll = function () {
    if (scrollY >= 400) {
        scrolll.style.display = "block";
    } else {
        scrolll.style.display = "none";
    }
}

function upScroll() {
    scroll({
        left: 0,
        top: 0,
        behavior: "smooth",
    })
}
document.addEventListener("DOMContentLoaded", function () {
    var addBtn = document.getElementById("MyAddBtn");
    console.log(addBtn); // المفروض يطبع الزرار

    if (addBtn) {
        addBtn.addEventListener("click", function () {
            console.log("Button clicked!"); // هل بيتطبع عند الضغط؟
            var myModal = new bootstrap.Modal(document.getElementById("addSlotModal"));
            myModal.show();
        });
    } else {
        console.error("Button not found!");
    }
});



document.addEventListener("DOMContentLoaded", function () {
    var addBtn = document.getElementById("Myaddslotbus");
    console.log(addBtn); // المفروض يطبع الزرار

    if (addBtn) {
        addBtn.addEventListener("click", function () {
            console.log("Button clicked!"); // هل بيتطبع عند الضغط؟
            var myModal = new bootstrap.Modal(document.getElementById("addSlotModal"));
            myModal.show();
        });
    } else {
        console.error("Button not found!");
    }
});



// function to remove values after closing the model 
function removevalue() {
    let mm = document.getElementById("addSlotModal")
    let value = document.getElementById("slotNameInputforadd").value = '';
    let errormessage = document.getElementById("nameValidationMessage").textContent = '';
}


let currentType = 'car'; // النوع الافتراضي عند فتح المودال
//const carAllowedLetters = /^[ABCabc]+$/;
//const busAllowedLetters = /^[XYZxyz]+$/;

function openModal(type) {
    document.getElementById("nameValidationMessage").textContent = '';
    document.getElementById("slotNameInputforadd").value = '';
    currentType = type;
    setType(type);
    $('#addSlotModal').modal('show'); // فتح المودال
}
function setType(type) {
    let slotInput = document.getElementById("slotNameInputforadd");

    // حفظ القيمة الحالية قبل تغيير الزر
    let lastValue = slotInput.value;

    // تغيير النوع
    currentType = type;
    document.getElementById("btnCar").classList.toggle("btn-primary", type === "car");
    document.getElementById("btnCar").classList.toggle("btn-outline-primary", type !== "car");

    document.getElementById("btnBus").classList.toggle("btn-danger", type === "bus");
    document.getElementById("btnBus").classList.toggle("btn-outline-danger", type !== "bus");

    // استعادة القيمة المخزنة بعد تغيير الزر
    slotInput.value = lastValue;

    // ✅ تحديث الفاليديشن فقط لو الإدخال مش فاضي
    if (slotInput.value.trim() !== "") {
        slotInput.dispatchEvent(new Event("input"));
    }
}


//function validateInput() {
//    const inputField = document.getElementById("slotNameInputforadd");
//    const validationMessage = document.getElementById("nameValidationMessage");
//    const saveButton = document.getElementById("saveSlotBtn");

//    const inputValue = inputField.value.trim();
//    const isValid = currentType === "car" ? carAllowedLetters.test(inputValue) : busAllowedLetters.test(inputValue);

//    if (inputValue.length === 0) {
//        validationMessage.textContent = "Slot name cannot be empty.";
//        saveButton.disabled = true;
//    } else if (!isValid) {
//        validationMessage.textContent = `Allowed letters: ${currentType === "car" ? "A, B, C" : "X, Y, Z"}`;
//        saveButton.disabled = true;
//    } else {
//        validationMessage.textContent = "";
//        saveButton.disabled = false;
//    }
//}



function submitSlot() {
    let slotNameInput = document.getElementById("slotNameInputforadd");
    let successMessageDiv = document.getElementById("name_Success_Message"); 
    if (!slotNameInput || !successMessageDiv) {
        console.error("❌ عنصر غير موجود:", { slotNameInput, successMessageDiv });
        return;
    }

    let slotName = slotNameInput.value.trim();

    if (!slotName) {
        console.warn("⚠️ Slot name is required.");
        return;
    }

    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/AdminDashboard/AddNewSlot", true);
    xhr.setRequestHeader("Content-Type", "application/json");

    xhr.onload = function () {
        console.log("📩 Raw Response:", xhr.responseText);

        if (xhr.status === 200) {
            try {
                let result = JSON.parse(xhr.responseText);
                console.log("✅ Parsed JSON:", result);

                if (result.success) {
                    // مسح الإدخال
                    slotNameInput.value = "";

                    // عرض رسالة النجاح
                    successMessageDiv.innerHTML = `✅ <span style="color: green;">Slotsss "${result.slotName}" added successfully!</span>`;
                    successMessageDiv.classList.remove("d-none");

                    // إخفاء الرسالة بعد 3 ثوانٍ
                    setTimeout(() => {
                        successMessageDiv.classList.add("d-none");
                    }, 5000);
                } else {
                    console.error("❌ Server error:", result.message);
                }
            } catch (error) {
                console.error("❌ JSON Parsing Error:", error);
            }
        } else {
            console.error("❌ Request failed. Status:", xhr.status);
        }
    };

    xhr.onerror = function () {
        console.error("❌ Network error.");
    };

    let formData = JSON.stringify({ parkingspot: { SlotName: slotName } });
    xhr.send(formData);
}

