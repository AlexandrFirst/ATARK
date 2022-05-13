import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { HttpBuildingService } from 'src/app/Services/httpBuilding.service';

@Component({
  selector: 'app-building-dialog',
  templateUrl: './building-dialog.component.html',
  styleUrls: ['./building-dialog.component.scss']
})
export class BuildingDialogComponent {


  id: number = null;

  buildingForm: FormGroup;

  get getForm() {
    return this.buildingForm;
  }

  get formAddress() {
    return this.buildingForm.get('address');
  }

  get formInfo() {
    return this.buildingForm.get('info');
  }

  constructor(public dialogRef: MatDialogRef<BuildingDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private buildingService: HttpBuildingService) {
    console.log(data)

    this.buildingForm = new FormGroup({
      address: new FormControl(data?.address, [Validators.required]),
      info: new FormControl(data?.info, [Validators.required])
    });

    if (data) {
      this.id = data.id;
    }
  }

  CloseAndChange() {
    if (this.buildingForm.valid) {
      this.buildingService.validateBuildingAdress(this.formAddress.value).subscribe(result => {
        console.log(this.buildingForm.value)
        this.dialogRef.close({ id: this.id, address: this.formAddress.value, info: this.formInfo.value, pos: result.location });
      }, error => {
        alert("No such address exists")
      });
    }
  }

  Close() {
    this.dialogRef.close(null);
  }
}
