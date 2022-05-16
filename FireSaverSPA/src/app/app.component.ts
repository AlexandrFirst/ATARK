import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import * as L from 'leaflet';
import { Observable, of, Subject, Subscriber } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ScalePointDto, Postion } from './Models/PointService/pointDtos';
import { HttpServiceService } from './Services/httpService.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  worldPosition: Postion;
  imgPosition: Postion;

  inputPos: Subject<Postion> = new Subject<Postion>();

  inputWorldPos: Subject<Postion> = new Subject<Postion>();

  mapsLoaded: Observable<boolean>;

  ngOnInit(): void {
  }

  constructor(private http: HttpServiceService, 
    httpClient: HttpClient) {

    this.mapsLoaded = httpClient.jsonp(`https://maps.googleapis.com/maps/api/js?key=${environment.googleMapsKey}`, 'callback')
      .pipe(map(() => true), catchError(() => of(false)))
  }

  worldMapCLicked($event: Postion) {
    console.log("world map postion: ", $event);
    this.worldPosition = $event;
  }

  myMapClicked($event: Postion) {
    console.log("my map postion: ", $event);
    this.imgPosition = $event;
  }

  writePostion() {
    if (!this.imgPosition || !this.worldPosition) {
      console.log("Select pos on both maps")
      return;
    }

    const pointToSend: ScalePointDto = {
      id: 0,
      mapPosition: this.imgPosition,
      worldPosition: this.worldPosition
    };

    this.http.sendPointData(pointToSend).subscribe(res => {
      console.log("Ok response: ", res);
    }, error => {
      console.log("smth went wrong ", error);
    })
  }

  calculateModel() {
    this.http.calculateModel().subscribe(res => {
      console.log("Ok response: ", res);
    }, error => {
      console.log("smth went wrong ", error);
    })
  }

  GetImageMapPosition() {
    if (!this.worldPosition) {
      console.log("Select postion on world map")
      return;
    }

    this.http.calculateImagePosition(this.worldPosition).subscribe((res: Postion) => {
      console.log("Ok response: ", res.latitude, res.longtitude);
      this.inputPos.next(res);
    }, error => {
      console.log("smth went wrong ", error);
    })
  }

  DeleteAllPoints() {
    this.http.deleteAllPoints().subscribe(res => {
      console.log("Ok response: ", res);
    }, error => {
      console.log("smth went wrong ", error);
    })
  }


  GetWorldMapPosition() {
    if (!this.imgPosition) {
      console.log("Select postion on image map")
      return;
    }

    this.http.calculateWorldPosition(this.imgPosition).subscribe((res: Postion) => {
      console.log("Ok response: ", res.latitude, res.longtitude);
      this.inputWorldPos.next(res);
    }, error => {
      console.log("smth went wrong ", error);
    })
  }
}