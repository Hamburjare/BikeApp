import { test, expect } from '@playwright/test';

test('test', async ({ page }) => {
  await page.goto('http://localhost:5173/');
  await page.getByRole('button', { name: 'open drawer' }).click();
  await page.getByRole('button', { name: 'Add Journey' }).click();
  await page.locator('button').nth(1).click();
  await page.getByPlaceholder('Departure Station ID').click();
  await page.getByPlaceholder('Departure Station ID').fill('113');
  await page.getByPlaceholder('Departure Station Name').click();
  await page.getByPlaceholder('Departure Station Name').fill('pasila');
  await page.getByPlaceholder('Departure Station Name').press('Tab');
  await page.getByPlaceholder('Return Station ID').fill('113');
  await page.getByPlaceholder('Return Station Name').click();
  await page.getByPlaceholder('Return Station Name').fill('pasila');
  await page.getByPlaceholder('Departure Time').click();
  await page.getByPlaceholder('Departure Time').press('Tab');
  await page.getByPlaceholder('Departure Time').fill('2023-04-16T14:54');
  await page.getByPlaceholder('Departure Time').press('Tab');
  await page.getByPlaceholder('Return Time').press('Tab');
  await page.getByPlaceholder('Return Time').fill('2023-03-16T14:59');
  await page.getByPlaceholder('Return Time').press('Tab');
  await page.getByPlaceholder('Covered Distance (m)').click();
  await page.getByPlaceholder('Covered Distance (m)').fill('634');
  await page.getByPlaceholder('Return Time').click();
  await page.getByPlaceholder('Return Time').click();
  await page.getByPlaceholder('Return Time').press('Tab');
  await page.getByPlaceholder('Return Time').fill('2023-04-16T14:59');
  page.once('dialog', dialog => {
    console.log(`Dialog message: ${dialog.message()}`);
    dialog.dismiss().catch(() => {});
  });
  await page.locator('form').getByRole('button', { name: 'Add Journey' }).click();
});