import { RequestHandler, RequestParamHandler } from 'express';
import swagggerUI from 'swagger-ui-express';
import swaggerDocument from 'src/swagger.json';

export const swaggerSetup: RequestHandler[] = [
	...swagggerUI.serve,
	swagggerUI.setup(swaggerDocument, {
		swaggerOptions: {
			validatorUrl: null
		}
	})
];
