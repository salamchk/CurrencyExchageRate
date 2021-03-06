var select = document.getElementById('currency-name');
var date = document.getElementById('date-field');
var text = document.getElementById('text-field');
text.innerHTML = select.value;

GetRates();

function GetRates() {
    select.innerHTML = '';
    fetch(DateToString(date.value), {
        method: 'POST',
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => response.json()).then(data => DataToSelect(data));
}

function DataToSelect(data) {
    data.forEach(item => {
        var currency = document.createElement('option');
        currency.value = item.rate;
        currency.innerHTML = item.fullName;
        select.appendChild(currency);
        text.innerHTML = select.value;
    });
}

function DateToString(date) {
    return new Date(date).toDateString();
}

select.addEventListener('change', (event) => {
    const selectedCurrency = event.target.value;
    text.innerHTML = selectedCurrency;
});



date.addEventListener('change', (event) => {
    GetRates();
})