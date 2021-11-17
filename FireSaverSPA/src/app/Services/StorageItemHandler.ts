import { ItemStorageHandler } from "./IItemStorageHandler";

export class StorageItemHandler implements ItemStorageHandler {

    private itemName: string = "";

    constructor(itemName: string) {
        this.itemName = itemName;
    }

    Write(item: string) {
        if (item)
            localStorage.setItem(this.itemName, item)
    }
    Read(): string {
        const retVal = localStorage.getItem(this.itemName).toString();
        if (!retVal)
            return "-1";
        return retVal;
    }
    Delete() {
        localStorage.removeItem(this.itemName);
    }

}