import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FloorDto } from 'src/app/Models/Compartment/floorDto';
import { FloorLevelValidator } from 'src/app/Validators/floorLevelValidator';
import { CompartmentAddDialogComponent } from '../compartment-add-dialog/compartment-add-dialog.component';

@Component({
  selector: 'app-floor-add-dialog',
  templateUrl: './floor-add-dialog.component.html',
  styleUrls: ['./floor-add-dialog.component.scss']
})
export class FloorAddDialogComponent extends CompartmentAddDialogComponent {

  private takenFloors: number[] = [];
  get formLevel() {
    return this.validatingForm.get('level')
  }

  constructor(dialogRef: MatDialogRef<CompartmentAddDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any) {
    super(dialogRef, data);

    this.addCompartmentForm.addControl('level', new FormControl('', [Validators.required]))

    if (data.takenFloors) {
      console.log(data.takenFloors)
      this.takenFloors = data.takenFloors
    }

    if (data.floorInfo) {
      const currentInfo: FloorDto = data.floorInfo;
      this.addCompartmentForm.setValue({
        name: currentInfo.name,
        description: currentInfo.description,
        safetyRules: currentInfo.safetyRules,
        level: currentInfo.level
      });
      const floorIndex = this.takenFloors.indexOf(data.floorInfo.level);
      this.takenFloors.splice(floorIndex, 1);
    }

    this.addCompartmentForm.get('level').setValidators([Validators.required, FloorLevelValidator(this.takenFloors)])
  }

}
