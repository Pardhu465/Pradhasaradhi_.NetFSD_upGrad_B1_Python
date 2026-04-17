

const questions = [
  { questionId:1, questionText:'2+2?', optionA:'3',optionB:'4',optionC:'5',optionD:'6', correctAnswer:'B' },
  { questionId:2, questionText:'Capital of France?', optionA:'London',optionB:'Berlin',optionC:'Paris',optionD:'Rome', correctAnswer:'C' },
  { questionId:3, questionText:'HTML stands for?', optionA:'Hyper Text Markup Language',optionB:'High Tech',optionC:'Hello Text',optionD:'None', correctAnswer:'A' },
  { questionId:4, questionText:'CSS means?', optionA:'Cascading Style Sheets',optionB:'Computer Sheets',optionC:'Creative Sheets',optionD:'Custom Sheets', correctAnswer:'A' },
];

describe('calculateScore', () => {
  test('100% when all correct',     () => expect(calculateScore(questions, {1:'B',2:'C',3:'A',4:'A'})).toBe(100));
  test('0% when all wrong',         () => expect(calculateScore(questions, {1:'A',2:'A',3:'B',4:'B'})).toBe(0));
  test('50% when 2 of 4 correct',   () => expect(calculateScore(questions, {1:'B',2:'C',3:'B',4:'B'})).toBe(50));
  test('0 for empty array',         () => expect(calculateScore([], {})).toBe(0));
  test('case-insensitive answers',  () => expect(calculateScore(questions, {1:'b',2:'c',3:'a',4:'a'})).toBe(100));
  test('partial answers handled',   () => expect(calculateScore(questions, {1:'B'})).toBe(25));
});

describe('getGrade', () => {
  test('A for 90+', () => expect(getGrade(90)).toBe('A'));
  test('A for 95',  () => expect(getGrade(95)).toBe('A'));
  test('B for 80',  () => expect(getGrade(80)).toBe('B'));
  test('C for 70',  () => expect(getGrade(70)).toBe('C'));
  test('D for 60',  () => expect(getGrade(60)).toBe('D'));
  test('F for 50',  () => expect(getGrade(50)).toBe('F'));
  test('F for 0',   () => expect(getGrade(0)).toBe('F'));
});

describe('filterQuestions', () => {
  test('returns all when no keyword',      () => expect(filterQuestions(questions,'').length).toBe(4));
  test('filters by keyword',               () => expect(filterQuestions(questions,'html').length).toBe(1));
  test('case-insensitive filter',          () => expect(filterQuestions(questions,'HTML').length).toBe(1));
  test('no match returns empty',           () => expect(filterQuestions(questions,'xyz').length).toBe(0));
});