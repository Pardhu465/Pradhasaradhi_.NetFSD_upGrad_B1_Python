function calculatePercentage(score, total) {
    return (score / total) * 100;
}

function checkPass(score, total) {
    let percent = calculatePercentage(score, total);

    if (percent >= 50) {
        return true;
    } else {
        return false;
    }
}

function calculateGrade(percent) {
    if (percent >= 80) {
        return "A";
    } else if (percent >= 60) {
        return "B";
    } else if (percent >= 50) {
        return "C";
    } else {
        return "F";
    }
}

module.exports = {
    calculatePercentage,
    checkPass,
    calculateGrade
};