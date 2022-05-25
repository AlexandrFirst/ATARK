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

  initBuildingAddr;

  Regions = [];

  get getForm() {
    return this.buildingForm;
  }

  get formAddress() {
    return this.buildingForm.get('address');
  }

  get formInfo() {
    return this.buildingForm.get('info');
  }

  get regionName() {
    return this.buildingForm.get('regionName');
  }

  constructor(public dialogRef: MatDialogRef<BuildingDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private buildingService: HttpBuildingService) {
    console.log(data)

    buildingService.getRegions().subscribe(data => {
      this.Regions = data;
    })

    this.buildingForm = new FormGroup({
      address: new FormControl(data?.address, [Validators.required]),
      info: new FormControl(data?.info, [Validators.required]),
      regionName: new FormControl(data?.region, [Validators.required])
    });

    if (data) {
      this.id = data.id;
    }
    this.initBuildingAddr = data?.address;
  }

  changeRegion(e: any) {
    this.regionName?.setValue(e.target.value, {
      onlySelf: true,
    });
  }

  CloseAndChange() {
    if (this.buildingForm.valid) {
      console.log("Selected region name: ", this.regionName)
      if (this.initBuildingAddr == this.formAddress.value) {
        this.dialogRef.close({ id: this.id, address: this.formAddress.value, info: this.formInfo.value, pos: null, region: this.regionName.value });
      }
      else {
        this.buildingService.validateBuildingAdress(this.formAddress.value).subscribe(result => {
          console.log(this.buildingForm.value)
          this.dialogRef.close({ id: this.id, address: this.formAddress.value, info: this.formInfo.value, pos: result.location, region: this.regionName.value });
        }, error => {
          alert("No such address exists")
        });
      }
    }
  }

  Close() {
    this.dialogRef.close(null);
  }
}
