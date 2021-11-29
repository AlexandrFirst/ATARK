import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CompartmentDto } from 'src/app/Models/Compartment/compartmentDto';
import { FloorDto } from 'src/app/Models/Compartment/floorDto';
import { HttpUserService } from 'src/app/Services/httpUser.service';
import { AsyncUserValidator } from 'src/app/Validators/asyncUserValidator';
import { FloorLevelValidator } from 'src/app/Validators/floorLevelValidator';

@Component({
  selector: 'app-compartment-add-dialog',
  templateUrl: './compartment-add-dialog.component.html',
  styleUrls: ['./compartment-add-dialog.component.scss']
})
export class CompartmentAddDialogComponent {

  addCompartmentForm: FormGroup = new FormGroup({
    name: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    safetyRules: new FormControl('', [Validators.required]),
  });

  get validatingForm() {
    return this.addCompartmentForm;
  }

  get formName() {
    return this.validatingForm.get('name');
  }

  get formDescription() {
    return this.validatingForm.get('description');
  }

  get formSafetyRules() {
    return this.validatingForm.get('safetyRules');
  }

  constructor(public dialogRef: MatDialogRef<CompartmentAddDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    
      if (data.roomInfo) {
        const currentInfo: CompartmentDto = data.roomInfo;
        this.addCompartmentForm.setValue({
          name: currentInfo.name,
          description: currentInfo.description,
          safetyRules: currentInfo.safetyRules
        });
      }
  }

  submitForm() {
    if (this.addCompartmentForm.valid) {
      this.dialogRef.close(this.addCompartmentForm.value)
    }
  }

  closeForm() {
    this.dialogRef.close();
  }

}
