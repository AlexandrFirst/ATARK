import { animate, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { LoaderSignServiceService } from 'src/app/Services/LoaderSignService.service';

@Component({
  selector: 'app-Loader',
  templateUrl: './Loader.component.html',
  styleUrls: ['./Loader.component.scss'],
  animations: [
    trigger('dialog', [
      transition('void => *', [
        style({ opacity: '0' }),
        animate('0.1s')
      ]),
      transition('* => void', [
        animate('0.1s', style({ opacity: '1' }))
      ])
    ])
  ]
})
export class LoaderComponent implements OnInit {

  constructor(private loadingService: LoaderSignServiceService) { }

  isActivated: boolean = false;

  ngOnInit() {
    this.loadingService.activateLoadSignStream.subscribe(next => {
      this.isActivated = next as boolean;
    });
  }

}
