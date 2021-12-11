import { Component, OnInit } from '@angular/core';
import { DeleteMessageDto } from 'src/app/Models/MessageModels/DeleteMessageDto';
import { MessageType, RecievedMessageDto } from 'src/app/Models/MessageModels/MessageRecievedDto';
import { HttpMessageServiceService } from 'src/app/Services/httpMessageService.service';
import { SignalRServiceService } from 'src/app/Services/SignalRService.service';

@Component({
  selector: 'app-UserMessages',
  templateUrl: './UserMessages.component.html'
})
export class UserNotificaionsComponent implements OnInit {

  messages: RecievedMessageDto[] = [];

  constructor(private socketService: SignalRServiceService,
    private messageService: HttpMessageServiceService) { }

  ngOnInit() {
    this.socketService.MessageRecieved().subscribe((message: RecievedMessageDto) => {
      this.messages.push(message);
    })

    this.socketService.MessageDelete().subscribe((delMessageInfo: DeleteMessageDto) => {
      this.deleteMessage(delMessageInfo.messageId);
    })

    this.messageService.getAllMessages().subscribe((messages: RecievedMessageDto[]) => {
      this.messages.push(...messages);
    })
  }

  deleteMessagedBtnClicked(messageId: number) {
    this.messageService.deleteMessage(messageId).subscribe(response => {
      this.deleteMessage(messageId);
    })
  }

  private deleteMessage(messageId: number) {
    var messageIdToDelete = messageId;
    var messageIndexToDelete = this.messages.findIndex(m => m.id == messageIdToDelete);
    this.messages.splice(messageIndexToDelete, 1);
  }

  checkType(messageType: MessageType): boolean{
    return messageType == MessageType.FIRE
  }

}
