export interface ItemStorageHandler{
    Write(item: any);
    Read():string;
    Delete();
}