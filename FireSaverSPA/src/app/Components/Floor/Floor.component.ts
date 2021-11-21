import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-Floor',
  templateUrl: './Floor.component.html',
  styleUrls: ['./Floor.component.scss']
})
export class FloorComponent implements OnInit {

  constructor(private location: Location) { }

  ngOnInit() {
  }

  backBtnClick() {
    this.location.back();
  }

}
