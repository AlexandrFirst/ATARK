import { Component, Inject, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
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
    @Inject(MAT_DIALOG_DATA) public data: any, private userService: HttpUserService) { }

  addQuestion() {
    const question = new FormGroup({
      content: new FormControl('', [Validators.required]),
      answers: new FormArray([], [Validators.required])
    })
    this.questions.push(question);
  }

  removeQuestion(questionIndex: number) {
    this.questions.removeAt(questionIndex);
  }

  removeAnswer(questionIndex: number, answerIndex: number) {
    this.answers(questionIndex).removeAt(answerIndex);
  }

  addAnswer(questionIndex: number) {
    const answer = new FormGroup({
      answer: new FormControl('', [Validators.required]),
      isTrue: new FormControl(false)
    })
    console.log("here")
    var answers = this.answers(questionIndex);
    console.log(answers)
    this.answers(questionIndex).push(answer);
  }
}
