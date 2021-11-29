import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-add-iot-dialog',
  templateUrl: './add-iot-dialog.component.html',
  styleUrls: ['./add-iot-dialog.component.scss']
})
export class AddIotDialogComponent {

  newIotForm: FormGroup = new FormGroup({
    iotIdField: new FormControl('', [Validators.required])
  })

  constructor(public dialogRef: MatDialogRef<AddIotDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  submitForm() {
    if (this.newIotForm.valid)
      this.dialogRef.close(this.newIotForm.value)
  }

  closeForm(){
    this.dialogRef.close();
  }

}
