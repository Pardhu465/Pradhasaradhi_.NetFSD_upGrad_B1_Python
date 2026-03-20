const { calculatePercentage, checkPass, calculateGrade } = require("./quizLogic");

test("should calculate score percentage correctly", () => {
    expect(calculatePercentage(4, 5)).toBe(80);
});

test("should return true when score is pass", () => {
    expect(checkPass(3, 5)).toBe(true);
});

test("should return correct grade based on percentage", () => {
    expect(calculateGrade(85)).toBe("A");
});