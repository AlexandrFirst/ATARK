import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoaderSignServiceService {

  constructor() { }

  activateLoadSignStream = new Subject();

  private isActivated: boolean = false;

  activate() {
    if (this.isActivated)
      return;
    this.isActivated = true;
    this.activateLoadSignStream.next(this.isActivated);
  }

  deactivate() {
    if (!this.isActivated)
      return;
    this.isActivated = false;
    this.activateLoadSignStream.next(this.isActivated);
  }

}
