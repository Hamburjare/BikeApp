import { test, expect } from '@playwright/test';

test('Test single station view', async ({ page }) => {
  await page.goto('http://localhost:8000/');
  await page.getByRole('button', { name: 'open drawer' }).click();
  await page.getByRole('button', { name: 'Stations' }).click();
  await page.locator('.css-1r9jet7 > .MuiButtonBase-root').click();
  await page.getByPlaceholder('Search').click();
  await page.getByPlaceholder('Search').fill('Pasila');
  await page.getByRole('button', { name: 'Search' }).click();
  await page.getByRole('row', { name: '113 Pasilan asema Böle station Pasilan asema-aukio Böle station Helsinki Helsingfors CityBike Finland 40 60,1982108580315 24,932799079033 View' }).getByRole('button', { name: 'View' }).click();
  await page.getByText('113');
  await page.getByText('Pasilan asema');
});