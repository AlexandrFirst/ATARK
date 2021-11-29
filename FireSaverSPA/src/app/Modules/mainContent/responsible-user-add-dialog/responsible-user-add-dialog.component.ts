import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { HttpUserService } from 'src/app/Services/httpUser.service';
import { AsyncUserValidator } from 'src/app/Validators/asyncUserValidator';

@Component({
  selector: 'app-responsible-user-add-dialog',
  templateUrl: './responsible-user-add-dialog.component.html',
  styleUrls: ['./responsible-user-add-dialog.component.scss']
})
export class ResponsibleUserAddDialogComponent {

  addResponsibleUserForm: FormGroup = new FormGroup({
    mail: new FormControl('',
      [Validators.required, Validators.email],
      [AsyncUserValidator.createCanUserBeResponsibleValidator(this.userService)])
  });

  get validatingForm() {
    return this.addResponsibleUserForm;
  }

  get formMail() {
    return this.addResponsibleUserForm.get('mail');
  }

  constructor(public dialogRef: MatDialogRef<ResponsibleUserAddDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private userService: HttpUserService) { }


  submitForm() {
    if (this.addResponsibleUserForm.valid)
      this.dialogRef.close(this.addResponsibleUserForm.value);
  }

  closeForm() {
    this.dialogRef.close();
  }
}
