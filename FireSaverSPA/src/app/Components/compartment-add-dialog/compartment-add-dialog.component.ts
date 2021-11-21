import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
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

  private takenFloors: number[] = [];

  addFloorToBuildingForm: FormGroup = new FormGroup({
    name: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    safetyRules: new FormControl('', [Validators.required]),
    level: new FormControl('', [Validators.required, FloorLevelValidator(this.takenFloors)])

  });

  get validatingForm() {
    return this.addFloorToBuildingForm;
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

  get formLevel() {
    return this.validatingForm.get('level')
  }



  constructor(public dialogRef: MatDialogRef<CompartmentAddDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private userService: HttpUserService) {
    if (data.takenFloors) {
      this.takenFloors = data.takenFloors
    }



    if (data.floorInfo) {
      const currentInfo: FloorDto = data.floorInfo;
      this.addFloorToBuildingForm.setValue({
        name: currentInfo.name,
        description: currentInfo.description,
        safetyRules: currentInfo.safetyRules,
        level: currentInfo.level
      });
      const floorIndex = this.takenFloors.indexOf(data.floorInfo.level);
      this.takenFloors.splice(floorIndex, 1);
    }
  }

  submitForm() {
    if (this.addFloorToBuildingForm.valid) {
      this.dialogRef.close(this.addFloorToBuildingForm.value)
    }
  }

  closeForm() {
    this.dialogRef.close();
  }

}
