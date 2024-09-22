import { Component, ElementRef, Input, ViewChild } from "@angular/core";
import { CommonModule } from "@angular/common";
import { QrcodeModule } from "qrcode-angular";
import { FormsModule } from "@angular/forms";

@Component({
	selector: "app-template-qr",
	standalone: true,
	imports: [CommonModule, QrcodeModule, FormsModule],
	templateUrl: "./template-qr.component.html",
	styleUrls: ["./template-qr.component.scss"],
})
export class TemplateQrComponent {
	value: string = "hehe";
	@ViewChild("templateContainer", { static: false })
	templateContainer!: ElementRef;

	downloadImage() {
		const container = this.templateContainer.nativeElement;
		const canvas = document.createElement("canvas");
		const ctx = canvas.getContext("2d");

		if (ctx) {
			const width = container.offsetWidth;
			const height = container.offsetHeight;

			// Đặt kích thước cho canvas
			canvas.width = width;
			canvas.height = height;

			// Vẽ hình ảnh nền lên canvas
			const img = new Image();
			img.src = container.querySelector("img").src;
			img.onload = () => {
				ctx.drawImage(img, 0, 0, width, height);

				// Vẽ mã QR lên canvas (copy kích thước và vị trí QR hiện tại)
				const qrCode = container.querySelector("qrcode");

				// const qrX = (width - qrCode.width) / 2;
				// const qrY = (height - qrCode.height) / 2;
				// ctx.drawImage(qrCode, qrX, qrY);

				// // Tải về hình ảnh canvas dưới dạng file
				// const link = document.createElement("a");
				// link.href = canvas.toDataURL("image/png");
				// link.download = "qr-code-template.png";
				// link.click();
			};
		}
	}
}
