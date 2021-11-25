import { Component, Inject, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { QuestionInput } from 'src/app/Models/TestModels/questionInput';
import { TestInput } from 'src/app/Models/TestModels/testInput';
import { HttpUserService } from 'src/app/Services/httpUser.service';

@Component({
  selector: 'app-test-dialog',
  templateUrl: './test-dialog.component.html',
  styleUrls: ['./test-dialog.component.scss']
})
export class TestDialogComponent {

  testForm: FormGroup = new FormGroup({
    tryCount: new FormControl('', [Validators.required]),
    questions: new FormArray([], [Validators.required])
  })

  get testTryCount() {
    return this.testForm.get('tryCount')
  }

  answerContent(questionIndex: number, answerIndex: number) {
    return this.answers(questionIndex).at(answerIndex).get('answer')
  }

  questionContent(questionIndex: number) {
    return this.questions.at(questionIndex).get('content');
  }

  get questions(): FormArray {
    return this.testForm.get("questions") as FormArray;
  }

  answers(questionIndex: number): FormArray {
    return this.questions.at(questionIndex).get('answers') as FormArray
  }

  constructor(public dialogRef: MatDialogRef<TestDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: TestInput, private userService: HttpUserService) {
    if (data) {
      console.log(data)
      this.initTestForm(data);
    }
  }

  addQuestion(value = null) {
    const question = new FormGroup({
      content: new FormControl('', [Validators.required]),
      answers: new FormArray([], [Validators.required])
    })
    if (value) {
      question.get('content').setValue(value);
    }

    this.questions.push(question);
  }

  removeQuestion(questionIndex: number) {
    this.questions.removeAt(questionIndex);
  }

  removeAnswer(questionIndex: number, answerIndex: number) {
    this.answers(questionIndex).removeAt(answerIndex);
  }

  addAnswer(questionIndex: number, value: { content, isTrue } = null) {
    const answer = new FormGroup({
      answer: new FormControl('', [Validators.required]),
      isTrue: new FormControl(false)
    })

    if (value) {
      answer.get('answer').setValue(value.content)
      answer.get('isTrue').setValue(value.isTrue)
    }

    var answers = this.answers(questionIndex);
    console.log(answers)
    this.answers(questionIndex).push(answer);
  }

  submitTest() {
    if (this.testForm.valid) {
      let testContet: TestInput = new TestInput()
      testContet.tryCount = this.testForm.get('tryCount').value
      testContet.questions = []
      this.testForm.get('questions').value.forEach(q => {
        let question: QuestionInput = new QuestionInput();
        question.content = q.content
        question.possibleAnswears = []
        question.answearsList = []
        q.answers.forEach(a => {
          let answerContent = a.answer;
          let isAnswerCorrect = a.isTrue;
          console.log(isAnswerCorrect, " ", a.isTrue)
          question.possibleAnswears.push(answerContent);
          if (isAnswerCorrect == true)
            question.answearsList.push(answerContent)
        });
        testContet.questions.push(question)
      });

      this.dialogRef.close(testContet)
    }
  }

  initTestForm(data: TestInput) {
    this.testForm.get('tryCount').setValue(data.tryCount)
    for (let i = 0; i < data.questions.length; i++) {
      this.addQuestion(data.questions[i].content);
      data.questions[i].possibleAnswears.forEach(possibleAnswer => {
        this.addAnswer(i,
          {
            content: possibleAnswer,
            isTrue: data.questions[i].answearsList.includes(possibleAnswer)
          })
      })
    }
  }
}
