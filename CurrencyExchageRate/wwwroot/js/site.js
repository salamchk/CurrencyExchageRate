const select = document.getElementById('currencyName');
const date = document.getElementById('date-field');
const text = document.getElementById('text-field');
text.innerHTML = select.value;

GetRates();

function GetRates() {
    select.innerHTML = ''; fetch(DateToString(date.value), {
        method: 'POST',
        headers: {'Content-Type': 'application/json;charset=utf-8'}}).then(response => response.json()).then(data => DataFromResponseToSelect(data));
}

function DataFromResponseToSelect(data) {data.forEach(item => {
        var currency = document.createElement('option');
        currency.value = item.rate;
        currency.innerHTML = item.fullName;
        select.appendChild(currency);
        text.innerHTML = select.value;
    })
}

function DateToString(date) {
    return new Date(date).toISOString();
}

select.addEventListener('change', (event) => {
    const selectedCurrency = event.target.value;
    text.innerHTML = selectedCurrency;
});



date.addEventListener('change', (event) => {
    GetRates();
})