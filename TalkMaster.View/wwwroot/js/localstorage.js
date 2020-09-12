
const storage = window.localStorage;


function strikeFactory(name, strikeCount = 1) {

    var date = Date.now();

    this.name = name;
    this.strikeCount = strikeCount;
    this.date = {
        day: date.getDay(),
        hour: date.getHours()
    }
    this.expiredDate = {
        day: date.getDay() + 1,
        hour: date.getHours()
    }

    if (hasStrike(name)) {
        updateStrike(this);
    } else {
        setStrike(this);
    }

    return this;
}

function getStrike(strike) {
    return storage.getItem(data.name);
}

function setStrike(strike) {
    storage.setItem(strike.name, strike.strikeCount);
}

function updateStrike(strike) {
    var item = storage.getItem(strike.name);
    storage.setItem(item.name, item.strikeCount + 1);
}

function hasStrike(strike) {
    return storage.getItem(strike.name) === undefined;
}

