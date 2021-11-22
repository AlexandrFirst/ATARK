import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormControlName, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-position-input-dialog',
  templateUrl: './position-input-dialog.component.html',
  styleUrls: ['./position-input-dialog.component.scss']
})
export class PositionInputDialogComponent {

  positionForm: FormGroup = new FormGroup({
    latitude: new FormControl('', [Validators.required]),
    longtitude: new FormControl('', [Validators.required]),
  });

  get formLatitude() {
    return this.positionForm.get('latitude');
  }

  get formLongtitude() {
    return this.positionForm.get('longtitude');
  }

  get formPosition() {
    return this.positionForm;
  }

  constructor(public dialogRef: MatDialogRef<PositionInputDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  submitData() {
    if (this.positionForm.valid) {
      this.dialogRef.close(this.positionForm.value)
    }
  }

  closeForm() {
    this.dialogRef.close();
  }



}
