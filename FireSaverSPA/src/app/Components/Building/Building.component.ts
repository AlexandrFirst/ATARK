import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import {Location} from '@angular/common';

@Component({
  selector: 'app-Building',
  templateUrl: './Building.component.html',
  styleUrls: ['./Building.component.scss']
})
export class BuildingComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private location: Location) { }

  ngOnInit() {
    this.activatedRoute.params.subscribe((params: Params) => {
      console.log(params)
    })
  }

  backBtnClick() {
    this.location.back();
  }

}
