export enum QRCodeType { CompartmentCode, UserCode, Undefined }

export class QRCodeFormat {
    userId: number = null;
    buildingId: number = null;
    IOTId: number = null;
    compatrmentId: number = null;
    QrCodeType: QRCodeType = QRCodeType.Undefined
}