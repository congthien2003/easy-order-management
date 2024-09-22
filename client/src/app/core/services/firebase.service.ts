import { Injectable } from "@angular/core";
import { AngularFireStorage } from "@angular/fire/compat/storage";
import { Observable, finalize } from "rxjs";
import { NgxImageCompressService, UploadResponse } from "ngx-image-compress";
import { Store } from "../models/interfaces/Store";
@Injectable({
	providedIn: "root",
})
export class FirebaseService {
	fileUpload!: File;
	compressedImage!: string;
	store!: Store;
	constructor(
		private storage: AngularFireStorage,
		private imageCompress: NgxImageCompressService
	) {
		this.store = JSON.parse(sessionStorage.getItem("storeInfo") ?? "");
	}

	// Hàm upload file lên Firebase
	uploadFileImage(file: File): Observable<string> {
		const filePath = `imagesClient/store_${
			this.store.id
		}/foods/${Date.now()}_${file.name}`; // Tạo đường dẫn cho file
		const fileRef = this.storage.ref(filePath);
		const task = this.storage.upload(filePath, file);
		return new Observable<string>((observer) => {
			task.snapshotChanges()
				.pipe(
					finalize(() => {
						fileRef.getDownloadURL().subscribe((downloadURL) => {
							observer.next(downloadURL); // Trả về URL tải file
							observer.complete();
						});
					})
				)
				.subscribe();
		});
	}

	async deleteFileFromFirebase(imageUrl: string): Promise<void> {
		const storageRef = this.storage.refFromURL(imageUrl); // Lấy tham chiếu đến file dựa trên URL
		return storageRef.delete().toPromise(); // Xóa file từ Firebase Storage
	}
}
