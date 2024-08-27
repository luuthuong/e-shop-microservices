export type PaymentStatus = 'pending' | 'success' | 'failed';

export class Payment {
    private readonly id!: string;
    private amount!: number;
    private userId!: string;
    private orderId!: string;
    private status: PaymentStatus = 'pending';

    public static create(id: string, amount: number, userId: string, orderId: string): Payment {
        const payment = new Payment();
        payment.amount = amount;
        payment.userId = userId;
        payment.orderId = orderId;
        return payment;
    }

    public getId(): string {
        return this.id;
    }

    public getAmount(): number {
        return this.amount!;
    }

    public getUserId(): string {
        return this.userId;
    }

    public getOrderId(): string {
        return this.orderId;
    }

    public getStatus(): PaymentStatus {
        return this.status;
    }

    public success(): void {
        this.status = 'success';
    }

    public fail(): void {
        this.status = 'failed';
    }
}
