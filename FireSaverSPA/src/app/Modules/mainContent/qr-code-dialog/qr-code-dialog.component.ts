import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-qr-code-dialog',
  templateUrl: './qr-code-dialog.component.html',
  styleUrls: ['./qr-code-dialog.component.scss']
})
export class QrCodeDialogComponent {

  elementType= "url"
  value: string = "";

  constructor(public dialogRef: MatDialogRef<QrCodeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { 
      this.value = data.info
    }

}
