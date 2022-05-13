import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ShelterDto } from 'src/app/Models/BuildingService/ShelterDto';
import { HttpBuildingService } from 'src/app/Services/httpBuilding.service';
import { AsyncAddressValidator } from 'src/app/Validators/adressValidator';

@Component({
  selector: 'app-shelter-dialog',
  templateUrl: './shelter-dialog.component.html',
  styleUrls: ['./shelter-dialog.component.scss']
})
export class ShelterDialogComponent {

  id: number = null;
  shelterForm: FormGroup;
  isAdressFromMap: boolean = false;

  constructor(
    public dialogRef: MatDialogRef<ShelterDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ShelterDto,
    private httpBuildingService: HttpBuildingService) {

    this.shelterForm = new FormGroup({
      address: new FormControl(data?.address, [Validators.required]),
      info: new FormControl(data?.info, [Validators.required]),
      capacity: new FormControl(data?.capacity, [Validators.required])
    })
  }

  get getForm() {
    return this.shelterForm;
  }

  get formAddress() {
    return this.shelterForm.get('address');
  }

  get formInfo() {
    return this.shelterForm.get('info');
  }

  get formCapacity() {
    return this.shelterForm.get('capacity');
  }

  submitForm() {
    if (this.shelterForm.valid) {

      this.httpBuildingService.validateBuildingAdress(this.formAddress.value).subscribe(data => {
        this.dialogRef.close({
          address: this.formAddress.value, info: this.formInfo.value, capacity: this.formCapacity.value, shelterPosition: {
            latitude: data.location.lat(),
            longtitude: data.location.lng()
          }
        });
      }, error => {
        alert('No adress exists')
        console.log(error)
      })
    }
  }

  closeForm() {
    this.dialogRef.close();
  }
}
